import React from "react"
import { Link } from "gatsby"
import Layout from "../components/layout"
import SEO from "../components/seo"

export default class App extends React.Component {
    state = {
        previous_url: "",
    }

    componentDidMount() {
        this.setState({ previous_url: window.location.pathname })
    }

    render() {
        return (
            <Layout>
                <SEO title="App" />
                <Link to="/" state={this.state}>
                    This should be a protected page....
                </Link>
            </Layout>
        )
    }
}
