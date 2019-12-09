import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import SEO from "../components/seo"

const Redirect = () => (
  <Layout>
    <SEO title="Redirect" />
    <Link to="/">Go back to the homepage</Link>
  </Layout>
)

export default Redirect
