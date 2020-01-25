import React from 'react';
import { Router } from '@reach/router';
import AppLayout from '../components/app_layout';
import PrivateRoute from '../components/privateRoute';
import Profile from '../components/profile';
import Login from '../components/login';
import { isAuthenticated } from '../services/auth';
import { navigate } from 'gatsby';

export const isBrowser = typeof window !== 'undefined';

const App = () => {
  if (isBrowser) {
    console.log('isBrowser');
    if (!isAuthenticated()) {
      console.log('not authhenticated');
      navigate('/');
    }
  }

  return (
    <>
      <AppLayout>
        <Router>
          <PrivateRoute path="/app/profile" component={Profile} />
          <Login path="/app/login" />
        </Router>
      </AppLayout>
    </>
  );
};

export default App;
