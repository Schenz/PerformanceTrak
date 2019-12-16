import React from "react"
import { Link } from "gatsby"
import Layout from "../components/layout"
import SEO from "../components/seo"

export default class About extends React.Component {
    state = {
        previous_url: "",
    }

    componentDidMount() {
        this.setState({ previous_url: window.location.pathname })
    }

    render() {
        return (
            <Layout>
                <SEO title="About" />
                <Link to="/" state={this.state}>
                    Go back to the homepage
                </Link>
            </Layout>
        )
    }
}
