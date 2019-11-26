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
                <li><a href="/About/">About us</a></li>
                <li><a href="/Contact/">Contact</a></li>
              </ul>
            </div>
          </div>
        
          <div class="col-md-3 col-sm-3">
            <h4 class="widget-title text-center">Products</h4>
            <div class="container">
              <div class="row">
                <div class="col-md-2 col-sm-2">
                  <div class="widget clearfix">
                      <ul>
                        <li><a href="/Rewards/">Rewards</a></li>
                        <li><a href="/PeerToPeer/">Peer to Peer</a></li>
                        <li><a href="/PerformanceImprovement/">Performance Improvement</a></li>
                        <li><a href="/BestPractices/">Best Practices</a></li>
                      </ul>
                  </div>
                </div>

                <div class="col-md-2 col-sm-2">
                  <div class="widget clearfix">
                    <ul>
                      <li><a href="/Safety/">Safety</a></li>
                      <li><a href="/Health/">Health</a></li>
                      <li><a href="/ContributionBonus/">Contribution Bonus</a></li>
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
            <p>{new Date().getFullYear()} &copy;<a href="/">perforamnceTrack</a> All rights reserved.</p>
          </div>
        </div>
      </div>
    </footer>
  </span>
)

export default Footer
