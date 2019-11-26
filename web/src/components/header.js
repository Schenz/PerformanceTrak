import { Link } from "gatsby"
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
            <Link class="navbar-brand" to="/"><img src={Logo} alt="PerformanceTrak" /></Link>
          </div>
          <div id="navbar" class="navbar-collapse collapse">
            <ul class="nav navbar-nav navbar-right">
              <li class="active"><Link to="/">Home</Link></li>
              <li class="dropdown yamm-fw hasmenu">
                <Link to="#top" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Products <span class="fa fa-angle-down"></span></Link>
                <ul class="dropdown-menu">
                  <li>
                    <div class="yamm-content">
                      <div class="row">
                        <div class="col-md-12">
                          <ul>
                            <li><Link to="/rewards/">Rewards</Link></li>
                            <li><Link to="/peertopeer/">Peer to Peer</Link></li>
                            <li><Link to="/performanceimprovement/">Performance Improvement</Link></li>
                            <li><Link to="/bestpractices/">Best Practices</Link></li>
                            <li><Link to="/yearsofservice/">Years Of Service</Link></li>
                            <li><Link to="/safety/">Safety</Link></li>
                            <li><Link to="/health/">Health</Link></li>
                            <li><Link to="/contributionbonus/">Contribution Bonus</Link></li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </li>
                </ul>
              </li>
              <li><Link to="/contact/">Contact us</Link></li>
              <li><Link to="/signin/">Sign In</Link></li>
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
