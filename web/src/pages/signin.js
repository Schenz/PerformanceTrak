import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import SEO from "../components/seo"

const Signin = () => (
  <Layout>
    <SEO title="Signin" />
    <Link to="/">Go back to the homepage</Link>
  </Layout>
)

export default Signin
