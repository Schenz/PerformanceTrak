import hello from 'hellojs';
import getRedirectUrl from './getRedirectUrl';
import './aadb2c.js';
import { navigate } from '@reach/router';

export const isBrowser = typeof window !== 'undefined';

export var loginDisplayType = {
  PopUp: 'popup',
  None: 'none',
  Page: 'page', //default is popup, if we use page option, webpage gets redirected to b2c login page then to redirect html.
};

export const isAuthenticated = () => {
  if (online()) {
    return tokens.idToken !== false;
  } else {
    return false;
  }
};

export const login = displayType => {
  stopTimer();

  if (!displayType) {
    displayType = loginDisplayType.Page;
  }

  hello('adB2CSignInSignUp')
    .login({ display: displayType, force: false })
    .then(
      function() {},
      function(e) {
        if ('Iframe was blocked' in e.error.message) {
          login(loginDisplayType.Page);
          return;
        }

        console.log(e);
      }
    );
};

export const logout = () => {
  stopTimer();
  if (isBrowser) {
    console.log('isBrowser');
    if (online()) {
      console.log('online');
      navigate('/');
      hello('adB2CSignInSignUp')
        .logout()
        .then(
          function() {
            console.log('Signed out');
          },
          function(e) {
            console.log('Signed out error: ' + e.error.message);
          }
        );
    } else {
      tokens.accessToken = false;
      tokens.idToken = false;
      user = {};
      window.localStorage.setItem('isLoggedIn', false);
      window.localStorage.removeItem('jwt');
      window.localStorage.removeItem('user');
      stopTimer();
      navigate('.');
    }
  }
};

export const getProfile = () => {
  return user;
};

let applicationId, scope, responseType, user, t, timer_is_on;

applicationId = '27d341d7-c9cf-409d-a134-cf8fe167463e';
scope = 'profile https://scdperformancetrak.onmicrosoft.com/PerformanceTrak/pt';
responseType = 'token id_token';
timer_is_on = false;

user = {};

const tokens = {
  idToken: false,
  accessToken: false,
};

if (isBrowser) {
  hello.init(
    {
      adB2CSignInSignUp: applicationId,
    },
    {
      redirect_uri: getRedirectUrl(),
      scope: 'openid ' + scope,
      response_type: responseType,
      response_mode: 'fragment',
    }
  );

  hello.on('auth.login', function(auth) {
    tokens.idToken = auth.authResponse.id_token;
    tokens.accessToken = auth.authResponse.access_token;
    window.localStorage.setItem('isLoggedIn', true);

    setUser(parseJwt(tokens.idToken));

    // TODO: Pass Bearer Auth Header
    if (user.isNew) {
      addUser();
    } else {
      getUser();
    }

    startTimer();
    navigate('/app/profile/');
  });

  hello.on('auth.logout', function(param) {
    tokens.accessToken = false;
    tokens.idToken = false;
    user = {};
    window.localStorage.setItem('isLoggedIn', false);
    window.localStorage.removeItem('jwt');
    window.localStorage.removeItem('user');
    stopTimer();
  });
}

function parseJwt(token) {
  return JSON.parse(
    decodeURIComponent(
      atob(
        token
          .split('.')[1]
          .replace(/-/g, '+')
          .replace(/_/g, '/')
      )
        .split('')
        .map(function(c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    )
  );
}

function setUser(jwt) {
  console.log(jwt);
  window.localStorage.setItem('jwt', JSON.stringify(jwt));

  user = {
    eTag: jwt.eTag || '',
    id: jwt.sub,
    name: jwt.name,
    family_name: jwt.family_name,
    given_name: jwt.given_name,
    city: jwt.city || '',
    country: jwt.country || '',
    postalCode: jwt.postalCode || '',
    state: jwt.state || '',
    streetAddress: jwt.streetAddress || '',
    isNew: jwt.newUser || false,
  };

  if (jwt.emails) {
    user.email = jwt.emails[0] || '';
  } else {
    user.email = jwt.email || '';
  }
  window.localStorage.setItem('user', JSON.stringify(user));
}

function online() {
  let session, currentTime;

  if (isBrowser) {
    session = hello('adB2CSignInSignUp').getAuthResponse();
    currentTime = new Date().getTime() / 1000; //seconds since 1 January 1970 00:00:00.

    return session && session.access_token && session.expires > currentTime;
  }
}

function timedCount() {
  if (!online()) {
    logout();
    stopTimer();
    return;
  }

  t = setTimeout(timedCount, 10000);
}

function startTimer() {
  if (!timer_is_on) {
    timedCount();
  }
}

function stopTimer() {
  clearTimeout(t);
}

function addUser() {
  const apiUrl = process.env.GATSBY_ADD_USER_FUNCTION_ENDPOINT;

  const options = {
    method: 'POST',
    body: JSON.stringify(user),
    headers: { 'Content-Type': 'application/json' },
  };

  fetch(apiUrl, options)
    .then(
      response => {
        if (response.ok) {
          return response.json();
        } else {
          return null;
        }
      },
      error => {
        console.error(error);
        navigate('/loginerror/');
      }
    )
    .then(data => {
      if (data !== null) {
        setUser(data);
      }
    });
}

function getUser() {
  const apiUrl = process.env.GATSBY_GET_USER_FUNCTION_ENDPOINT;

  const options = {
    method: 'GET',
    headers: { 
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${tokens.accessToken}`
    },
  };

  var partitionKey = user.family_name.substring(0, 1);
  var rowKey = user.id;

  fetch(`${apiUrl}/${partitionKey}/${rowKey}`, options)
    .then(
      response => {
        if (response.ok) {
          return response.json();
        } else {
          return null;
        }
      },
      error => {
        console.error(error);
        navigate('/loginerror/');
      }
    )
    .then(data => {
      console.log("Set User from API result: ", data)
      if (data !== null) {
        setUser(data);
      }
    });
}

startTimer();
