import { Link } from "gatsby"
import React from "react"

const Footer = () => (
  <span>
    <footer class="footer primary-footer">
      <div class="container">
        <div class="row">
          <div class="col-md-2 col-sm-2"></div>
        
          <div class="col-md-4 col-sm-4">
            <div class="widget clearfix">
              <h4 class="widget-title">Company</h4>
              <ul>
                <li><Link to="/about/">About us</Link></li>
                <li><Link to="/contact/">Contact</Link></li>
              </ul>
            </div>
          </div>
        
          <div class="col-md-4 col-sm-4">
            <h4 class="widget-title text-center">Products</h4>
            <div class="container">
              <div class="row">
                <div class="col-md-3 col-sm-3">
                  <div class="widget clearfix">
                      <ul>
                        <li><Link to="/rewards/">Rewards</Link></li>
                        <li><Link to="/peertopeer/">Peer to Peer</Link></li>
                        <li><Link to="/performanceimprovement/">Performance Improvement</Link></li>
                        <li><Link to="/bestpractices/">Best Practices</Link></li>
                      </ul>
                  </div>
                </div>

                <div class="col-md-2 col-sm-2">
                  <div class="widget clearfix">
                    <ul>
                      <li><Link to="/safety/">Safety</Link></li>
                      <li><Link to="/health/">Health</Link></li>
                      <li><Link to="/contributionbonus/">Contribution Bonus</Link></li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </footer>
  
    <footer class="footer secondary-footer">
      <div class="container">
        <div class="row">
          <div class="col-md-6 col-sm-6 col-xs-12">
            <p>{new Date().getFullYear()} &copy;<Link to="/">PerformanceTrak</Link> All rights reserved.</p>
          </div>
        </div>
      </div>
    </footer>
  </span>
)

export default Footer
