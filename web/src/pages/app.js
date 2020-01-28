import React from 'react';
import { Router } from '@reach/router';
import AppLayout from '../components/App/layout';
import PrivateRoute from '../components/App/privateRoute';
import Profile from '../components/App/profile';
import Login from '../components/login';
import { isAuthenticated } from '../services/auth';
import { navigate } from 'gatsby';
import ProfileUpdated from '../components/App/profile_updated';

export const isBrowser = typeof window !== 'undefined';

const App = () => {
  if (isBrowser) {
    if (!isAuthenticated()) {
      navigate('/');
    }
  }

  return (
    <>
      <AppLayout>
        <Router>
          <PrivateRoute path="/app/profile" component={Profile} />
          <PrivateRoute path="/app/app_profile_updated" component={ProfileUpdated} />
          <Login path="/app/login" />
        </Router>
      </AppLayout>
    </>
  );
};

export default App;
