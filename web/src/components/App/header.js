import { Link } from 'gatsby';
import React from 'react';
import Logo from '../images/logo.png';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { login, loginDisplayType, logout, isAuthenticated, getUser } from '../../services/auth';

export default class AppHeader extends React.Component {
  state = {
    previous_url: '',
  };

  setActiveMenuItem(pathName) {
    /*let listItem;

    if (pathName === '/') {
      listItem = document.querySelector('#navHome');
      listItem.classList.add('active');
    } else if (pathName === '/signin/') {
      listItem = document.querySelector('#navSignin');
      listItem.classList.add('active');
    } else if (pathName === '/contact/' || pathName === '/thankyou/') {
      listItem = document.querySelector('#navContact');
      listItem.classList.add('active');
    } else {
      if (pathName !== '/404/') {
        listItem = document.querySelector('#navProducts');
        listItem.classList.add('active');
      }
    }*/
  }

  async trackPageView(pathName) {
    let previous_url;

    if (window.history.state == null) {
      previous_url = '';
    } else {
      previous_url = window.history.state.previous_url;
    }

    const appInsights = new ApplicationInsights({
      config: {
        instrumentationKey: process.env.GATSBY_INSTRUMENTATION_KEY,
      },
    });

    appInsights.loadAppInsights();

    if (isAuthenticated()) {
      appInsights.setAuthenticatedUserContext(await getUser().id);
    } else {
      appInsights.clearAuthenticatedUserContext();
    }
    appInsights.trackPageView({
      uri: window.location.href,
      name: pathName,
      refUri: previous_url,
    });
  }

  componentDidMount() {
    this.setState({ previous_url: window.location.pathname });
    this.setActiveMenuItem(window.location.pathname);
    this.trackPageView(window.location.pathname);
  }

  render() {
    return (
      <header className="header site-header">
        <div className="container">
          <nav className="navbar navbar-default yamm">
            <div className="container-fluid">
              <div className="navbar-header">
                <button
                  type="button"
                  className="navbar-toggle collapsed"
                  data-toggle="collapse"
                  data-target="#navbar"
                  aria-expanded="false"
                  aria-controls="navbar"
                >
                  <span className="sr-only">Toggle navigation</span>
                  <span className="icon-bar"></span>
                  <span className="icon-bar"></span>
                  <span className="icon-bar"></span>
                </button>
                <Link className="navbar-brand" to="/" state={this.state}>
                  <img src={Logo} alt="PerformanceTrak" />
                </Link>
              </div>
              <div id="navbar" className="navbar-collapse collapse">
                <ul className="nav navbar-nav navbar-right">
                  <li id="navHome">
                    <Link to="/" state={this.state}>
                      Home
                    </Link>
                  </li>
                  <li id="navProfile">
                    <Link to="/app/profile/" state={this.state}>
                      Profile
                    </Link>
                  </li>
                  <li id="navSignin">
                    {isAuthenticated() ? (
                      <Link
                        to="."
                        onClick={e => {
                          logout();
                          e.preventDefault();
                        }}
                      >
                        Sign Out
                      </Link>
                    ) : (
                      <Link
                        to="."
                        onClick={e => {
                          login(loginDisplayType.PopUp);
                          e.preventDefault();
                        }}
                      >
                        Sign In
                      </Link>
                    )}
                  </li>
                </ul>
              </div>
            </div>
          </nav>
        </div>
      </header>
    );
  }
}
