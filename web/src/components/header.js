import { Link } from 'gatsby';
import React from 'react';
import Logo from './images/logo.png';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { login, loginDisplayType, logout } from '../services/auth';

export default class Header extends React.Component {
  state = {
    previous_url: '',
  };

  setActiveMenuItem(pathName) {
    let listItem;

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
    }
  }

  trackPageView(pathName) {
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
                  <li id="navProducts" className="dropdown yamm-fw hasmenu">
                    <Link
                      to="#top"
                      className="dropdown-toggle"
                      data-toggle="dropdown"
                      role="button"
                      aria-haspopup="true"
                      aria-expanded="false"
                      state={this.state}
                    >
                      Products <span className="fa fa-angle-down"></span>
                    </Link>
                    <ul className="dropdown-menu">
                      <li>
                        <div className="yamm-content">
                          <div className="row">
                            <div className="col-md-12">
                              <ul>
                                <li>
                                  <Link to="/rewards/" state={this.state}>
                                    Rewards
                                  </Link>
                                </li>
                                <li>
                                  <Link to="/peertopeer/" state={this.state}>
                                    Peer to Peer
                                  </Link>
                                </li>
                                <li>
                                  <Link to="/performanceimprovement/" state={this.state}>
                                    Performance Improvement
                                  </Link>
                                </li>
                                <li>
                                  <Link to="/bestpractices/" state={this.state}>
                                    Best Practices
                                  </Link>
                                </li>
                                <li>
                                  <Link to="/yearsofservice/" state={this.state}>
                                    Years Of Service
                                  </Link>
                                </li>
                                <li>
                                  <Link to="/safety/" state={this.state}>
                                    Safety
                                  </Link>
                                </li>
                                <li>
                                  <Link to="/health/" state={this.state}>
                                    Health
                                  </Link>
                                </li>
                                <li>
                                  <Link to="/contributionbonus/" state={this.state}>
                                    Contribution Bonus
                                  </Link>
                                </li>
                              </ul>
                            </div>
                          </div>
                        </div>
                      </li>
                    </ul>
                  </li>
                  <li id="navContact">
                    <Link to="/contact/" state={this.state}>
                      Contact us
                    </Link>
                  </li>
                  <li id="navSignin">
                    <button
                      onClick={e => {
                        
                        login(loginDisplayType.PopUp);
                        e.preventDefault();
                      }}
                    >
                      Sign In
                    </button>
                    <button
                      onClick={e => {
                        logout();
                        e.preventDefault();
                      }}
                    >
                      Sign Out
                    </button>
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
