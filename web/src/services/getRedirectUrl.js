export const isBrowser = typeof window !== 'undefined';

export default function getRedirectUrl(){
  let port;

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

    return `${window.location.protocol}//${window.location.hostname}${port}`;
  }
};
