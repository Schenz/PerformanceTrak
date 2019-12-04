import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import SEO from "../components/seo"

const ThankYou = () => (
  <Layout>
    <SEO title="Thank You" />
    <section className="section overfree">
      <div className="container">
        <div className="section-title text-center">
          <h3>Thank you!</h3>
          <hr />
          <p className="lead">We have received your message and someone will get back with you as soon as possible.</p>
        </div>
      </div>
    </section>
  </Layout>
)

export default ThankYou