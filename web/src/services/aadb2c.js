import hello from 'hellojs';
import getRedirectUrl from './getRedirectUrl';

let tenant, tenantDomain;

export const isBrowser = typeof window !== 'undefined';

tenant = 'scdperformancetrak';
tenantDomain = `${tenant}.onmicrosoft.com`;

const policy = process.env.GATSBY_AAD_POLICY;

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
        logout: function() {
          let id_token;

          //get id_token from auth response
          id_token = hello('adB2CSignInSignUp').getAuthResponse().id_token;

          //clearing local storage session
          hello.utils.store('adB2CSignInSignUp', null);

          //redirecting to Azure B2C logout URI
          window.location = `https://${tenant}.b2clogin.com/${tenantDomain}/${policy}/oauth2/v2.0/logout?id_token_hint=${id_token}&post_logout_redirect_uri=${getRedirectUrl()}`;
        },
        xhr: function(p) {
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
