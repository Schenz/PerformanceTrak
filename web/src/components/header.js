import { Link } from "gatsby"
import React from "react"
import Logo from "./images/logo.png"

export default class Header extends React.Component {
  componentDidMount() {
    var pathName, listItem;

    pathName = window.location.pathname;
    
    if (pathName === "/") {
      listItem = document.querySelector("#navHome")
      listItem.classList.add("active");
    } else if (pathName === "/signin/") {
      listItem = document.querySelector("#navSignin")
      listItem.classList.add("active");
    } else if (pathName === "/contact/") {
      listItem = document.querySelector("#navContact")
      listItem.classList.add("active");
    } else {
      if (pathName !== "/404/") {
        listItem = document.querySelector("#navProducts")
        listItem.classList.add("active");
      }
    }
  }

  render() {
    return (
      <header className="header site-header">
        <div className="container">
          <nav className="navbar navbar-default yamm">
            <div className="container-fluid">
              <div className="navbar-header">
                <button type="button" className="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
				          <span className="sr-only">Toggle navigation</span>
				          <span className="icon-bar"></span>
				          <span className="icon-bar"></span>
				          <span className="icon-bar"></span>
				        </button>
                <Link className="navbar-brand" to="/"><img src={Logo} alt="PerformanceTrak" /></Link>
              </div>
              <div id="navbar" className="navbar-collapse collapse">
                <ul className="nav navbar-nav navbar-right">
                  <li id="navHome"><Link to="/">Home</Link></li>
                  <li id="navProducts" className="dropdown yamm-fw hasmenu">
                    <Link to="#top" className="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Products <span className="fa fa-angle-down"></span></Link>
                    <ul className="dropdown-menu">
                      <li>
                        <div className="yamm-content">
                          <div className="row">
                            <div className="col-md-12">
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
                  <li id="navContact"><Link to="/contact/">Contact us</Link></li>
                  <li id="navSignin"><Link to="/signin/">Sign In</Link></li>
                </ul>
              </div>
            </div>
          </nav>
        </div>
      </header>
    )
  }
}