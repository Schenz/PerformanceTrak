import * as hello from 'hellojs';

let tenant, tenantDomain, policy, redirect_uri, port, helloJsSignInSignUpPolicy;

export const isBrowser = typeof window !== 'undefined';

tenant = 'scdperformancetrak';
tenantDomain = `${tenant}.onmicrosoft.com`;
policy = 'B2C_1_SingUpSignIn2';

if (isBrowser) {
  if (
    window.location.port === '80' ||
    window.location.port === '' ||
    window.location.port === '0' ||
    window.location.port === 80 ||
    window.location.port === 0
  ) {
    port = '/';
  } else {
    port = ':' + window.location.port + '/';
  }
  redirect_uri = window.location.protocol + '//' + window.location.hostname + port + 'redirect/';

  console.log('redirect_uri in aadb2c.js: ', redirect_uri);
}

helloJsSignInSignUpPolicy = 'adB2CSignInSignUp';

if (isBrowser) {
  (function(hello) {
    hello.init({
      adB2CSignInSignUp: {
        name: 'Azure Active Directory B2C',
        oauth: {
          version: 2,
          auth: `https://${tenant}.b2clogin.com/${tenantDomain}/${policy}/oauth2/v2.0/authorize`,
          grant: `https://${tenant}.b2clogin.com/${tenantDomain}/${policy}/oauth2/v2.0/token`,
        },
        refresh: true,
        scope_delim: ' ',
        base: `https://graph.windows.net/${tenantDomain}/`,
        get: {
          me: 'me?api-version=1.6',
        },
        logout: function() {
          let id_token;

          //get id_token from auth response
          id_token = hello(helloJsSignInSignUpPolicy).getAuthResponse().id_token;

          //clearing local storage session
          hello.utils.store(helloJsSignInSignUpPolicy, null);

          //redirecting to Azure B2C logout URI
          //window.location = `https://login.microsoftonline.com/${tenantDomain}/oauth2/v2.0/logout?p=${policy}&id_token_hint=${id_token}&post_logout_redirect_uri=${redirect_uri}`;
          window.location = `https://${tenant}.b2clogin.com/${tenantDomain}/${policy}/oauth2/v2.0/logout?id_token_hint=${id_token}&post_logout_redirect_uri=${redirect_uri}`;
        },
        xhr: function(p) {
          console.log('IN AADB2C.JS!!!');
          console.log(p);
          let token;

          token = p.query.access_token;
          delete p.query.access_token;

          if (token) {
            p.headers = {
              Authorization: `Bearer ${token}`,
            };
          }

          if (p.method === 'post' || p.method === 'put') {
            if (typeof p.data === 'object') {
              // Convert the POST into a javascript object
              try {
                p.data = JSON.stringify(p.data);
                p.headers['content-type'] = 'application/json';
              } catch (e) {}
            }
          } else if (p.method === 'patch') {
            hello.utils.extend(p.query, p.data);
            p.data = null;
          }

          return true;
        },
        // Don't even try submitting via form.
        // This means no POST operations in <=IE9
        form: false,
      },
    });
  })(hello);
}
