import * as hello from 'hellojs';
import './aadb2c.js';

export const isBrowser = typeof window !== 'undefined';

export var loginDisplayType = {
  PopUp: 'popup',
  None: 'none',
  Page: 'page', //default is popup, if we use page option, webpage gets redirected to b2c login page then to redirect html.
};

export const isAuthenticated = () => {
  return tokens.idToken !== false;
};

export const login = displayType => {
  console.log('Enter login at: ' + new Date());
  console.log('displayType: ' + displayType);

  if (!displayType) {
    displayType = loginDisplayType.Page;
  }

  //in case of silent renew, check if the session is still active otherwise ask the user to login again
  if (!online() && displayType === loginDisplayType.None) {
    console.log('silent refresh');

    hello('adB2CSignInSignUp')
      .login({ display: displayType, force: false })
      .then(
        function() {
          console.log('Silent Nothing Function...');
        },
        function(e) {
          console.log(e);
          if ('Iframe was blocked' in e.error.message) {
            login(loginDisplayType.Page);
            return;
          }

          alert('Signin error: ' + e.error.message);
        }
      );

    return;
  }

  hello('adB2CSignInSignUp')
    .login({ display: displayType, force: false })
    .then(
      function() {
        console.log('Do Nothing Function...');
      },
      function(e) {
        console.log(e);
        if ('Iframe was blocked' in e.error.message) {
          login(loginDisplayType.Page);
          return;
        }

        alert('Signin error: ' + e.error.message);
      }
    );
};

export const logout = () => {
  console.log('Enter logout at: ' + new Date());
  if (online()) {
    hello('adB2CSignInSignUp')
      .logout({ force: true })
      .then(
        function() {
          alert('policy: B2C_1_SingUpSignIn2 You are logging out from AD B2C');
        },
        function(e) {
          alert('Logout error: ' + e.error.message);
        }
      );
  }
};

export const getProfile = () => {
  return user;
};

let applicationId, scope, responseType, redirect_uri, user, port;

applicationId = '27d341d7-c9cf-409d-a134-cf8fe167463e';
scope = 'https://scdperformancetrak.onmicrosoft.com/PerformanceTrak/pt';
responseType = 'token id_token';
if (isBrowser) {
  if (window.location.port === '80') {
    port = '';
  } else {
    port = window.location.port;
  }
  redirect_uri = window.location.protocol + '//' + window.location.hostname + '/' + port + '/redirect/';
}
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
      redirect_uri: redirect_uri,
      scope: 'openid ' + scope,
      response_type: responseType,
    }
  );

  hello.on('auth.login', function(auth) {
    console.log('Enter auth.login at: ' + new Date());

    tokens.idToken = auth.authResponse.id_token;
    tokens.accessToken = auth.authResponse.access_token;
    window.localStorage.setItem('isLoggedIn', false);

    var jwt = parseJwt(tokens.idToken);
    console.log(jwt);
    setUser(jwt);
    console.log(user);
  });

  hello.on('auth.logout', function() {
    console.log('Enter auth.logout at: ' + new Date());
    tokens.accessToken = false;
    tokens.idToken = false;
    user = {};
    window.localStorage.setItem('isLoggedIn', false);
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
  user = {
    name: jwt.name,
    family_name: jwt.family_name,
    given_name: jwt.given_name,
    city: jwt.city,
    country: jwt.country,
    postalCode: jwt.postalCode,
    state: jwt.state,
    streetAddress: jwt.streetAddress,
    emails: jwt.emails,
  };
}

function online() {
  console.log('enter online at: ' + new Date());

  let session, currentTime;

  session = hello('adB2CSignInSignUp').getAuthResponse();
  currentTime = new Date().getTime() / 1000; //seconds since 1 January 1970 00:00:00.

  return session && session.access_token && session.expires > currentTime;
}
