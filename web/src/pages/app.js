import React from 'react';
import { Router } from '@reach/router';
import AppLayout from '../components/app_layout';
import PrivateRoute from '../components/privateRoute';
import Profile from '../components/profile';
import Login from '../components/login';
import { isAuthenticated } from '../services/auth';
import { navigate } from 'gatsby';

const App = () => {
  if (!isAuthenticated()) {
    navigate('/');
  } else {
    navigate('/app/profile/');
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
