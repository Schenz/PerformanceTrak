import { Link } from "gatsby"
import React from "react"

export default class Footer extends React.Component {
    state = {
        previous_url: "",
    }

    componentDidMount() {
        this.setState({ previous_url: window.location.pathname })
    }

    render() {
        return (
            <span>
                <footer className="footer primary-footer">
                    <div className="container">
                        <div className="row">
                            <div className="col-md-2 col-sm-2"></div>

                            <div className="col-md-4 col-sm-4">
                                <div className="widget clearfix">
                                    <h4 className="widget-title">Company</h4>
                                    <ul>
                                        <li>
                                            <Link
                                                to="/about/"
                                                state={this.state}
                                            >
                                                About us
                                            </Link>
                                        </li>
                                        <li>
                                            <Link
                                                to="/contact/"
                                                state={this.state}
                                            >
                                                Contact
                                            </Link>
                                        </li>
                                    </ul>
                                </div>
                            </div>

                            <div className="col-md-4 col-sm-4">
                                <h4 className="widget-title text-center">
                                    Products
                                </h4>
                                <div className="container">
                                    <div className="row">
                                        <div className="col-md-3 col-sm-3">
                                            <div className="widget clearfix">
                                                <ul>
                                                    <li>
                                                        <Link
                                                            to="/rewards/"
                                                            state={this.state}
                                                        >
                                                            Rewards
                                                        </Link>
                                                    </li>
                                                    <li>
                                                        <Link
                                                            to="/peertopeer/"
                                                            state={this.state}
                                                        >
                                                            Peer to Peer
                                                        </Link>
                                                    </li>
                                                    <li>
                                                        <Link
                                                            to="/performanceimprovement/"
                                                            state={this.state}
                                                        >
                                                            Performance
                                                            Improvement
                                                        </Link>
                                                    </li>
                                                    <li>
                                                        <Link
                                                            to="/bestpractices/"
                                                            state={this.state}
                                                        >
                                                            Best Practices
                                                        </Link>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>

                                        <div className="col-md-2 col-sm-2">
                                            <div className="widget clearfix">
                                                <ul>
                                                    <li>
                                                        <Link
                                                            to="/yearsofservice/"
                                                            state={this.state}
                                                        >
                                                            Years of Service
                                                        </Link>
                                                    </li>
                                                    <li>
                                                        <Link
                                                            to="/safety/"
                                                            state={this.state}
                                                        >
                                                            Safety
                                                        </Link>
                                                    </li>
                                                    <li>
                                                        <Link
                                                            to="/health/"
                                                            state={this.state}
                                                        >
                                                            Health
                                                        </Link>
                                                    </li>
                                                    <li>
                                                        <Link
                                                            to="/contributionbonus/"
                                                            state={this.state}
                                                        >
                                                            Contribution Bonus
                                                        </Link>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </footer>

                <footer className="footer secondary-footer">
                    <div className="container">
                        <div className="row">
                            <div className="col-md-6 col-sm-6 col-xs-12">
                                <p>
                                    {new Date().getFullYear()} &copy;
                                    <Link to="/" state={this.state}>
                                        PerformanceTrak
                                    </Link>{" "}
                                    All rights reserved.
                                </p>
                            </div>
                        </div>
                    </div>
                </footer>
            </span>
        )
    }
}
