import React from "react"

import Layout from "../components/layout"
import SEO from "../components/seo"

const EmailError = () => (
    <Layout>
        <SEO title="Email Error" />
        <section className="section overfree">
            <div className="container">
                <div className="section-title text-center">
                    <h3>An error Occurred!</h3>
                    <hr />
                    <p className="lead">
                        We were unable to get your message, please try again in a few minutes.
                    </p>
                </div>
            </div>
        </section>
    </Layout>
)

export default EmailError
