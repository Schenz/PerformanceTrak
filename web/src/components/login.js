import React from 'react';
import { navigate } from 'gatsby';
import { isAuthenticated, login, loginDisplayType } from '../services/auth';

class Login extends React.Component {
  componentDidMount() {
    console.log('componentDidMount');

    if (isAuthenticated()) {
      console.log('isAuthenticated');
      navigate(`/app/profile`);
    } else {
      console.log('login');
      login(loginDisplayType.PopUp);
    }
  }

  render() {
    return <></>;
  }
}

export default Login;
