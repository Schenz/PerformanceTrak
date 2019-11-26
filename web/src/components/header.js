//import { Link } from "gatsby"
import PropTypes from "prop-types"
import React from "react"
import Logo from "./images/logo.png"

const Header = ({ siteTitle }) => (
  <header class="header site-header">
    <div class="container">
      <nav class="navbar navbar-default yamm">
        <div class="container-fluid">
          <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
				      <span class="sr-only">Toggle navigation</span>
				      <span class="icon-bar"></span>
				      <span class="icon-bar"></span>
				      <span class="icon-bar"></span>
				    </button>
            <a class="navbar-brand" href="/"><img src={Logo} alt="PerformanceTrak" /></a>
          </div>
          <div id="navbar" class="navbar-collapse collapse">
            <ul class="nav navbar-nav navbar-right">
              <li class="active"><a href="/">Home</a></li>
              <li class="dropdown yamm-fw hasmenu">
                <a href="#top" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Products <span class="fa fa-angle-down"></span></a>
                <ul class="dropdown-menu">
                  <li>
                    <div class="yamm-content">
                      <div class="row">
                        <div class="col-md-4">
                          <ul>
                            <li><a href="/Rewards/">Rewards</a></li>
                            <li><a href="/PeerToPeer/">Peer to Peer</a></li>
                            <li><a href="/PerformanceImprovement/">Performance Improvement</a></li>
                            <li><a href="/BestPractices/">Best Practices</a></li>
                            <li><a href="/YearsOfService/">Years Of Service</a></li>
                            <li><a href="/Safety/">Safety</a></li>
                            <li><a href="/Health/">Health</a></li>
                            <li><a href="/ContributionBonus/">Contribution Bonus</a></li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </li>
                </ul>
              </li>
              <li><a href="/Contact/">Contact us</a></li>
              <li><a href="/Signin/">Sign In</a></li>
            </ul>
          </div>
        </div>
      </nav>
    </div>
  </header>
)

Header.propTypes = {
  siteTitle: PropTypes.string,
}

Header.defaultProps = {
  siteTitle: ``,
}

export default Header
