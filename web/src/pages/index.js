import React from "react"

import Layout from "../components/layout"
import SEO from "../components/seo"

const IndexPage = () => (
  <Layout>
    <SEO title="Home" />
    <section class="section overfree">
      <div class="container">
        <div class="section-title text-center">
          <small>Welcome to the best SEO service</small>
          <h3>Top Reasons to Choose Us</h3>
          <hr />
          <p class="lead">Our cloud-based, SaaS platform helps our global clients align, engage, and recognize their employees.</p>
        </div>
        
        <div class="row service-list text-center">
          <div class="col-md-4 col-sm-12 col-xs-12 first">
            <div class="service-wrapper wow fadeIn">
              <i class="flaticon-competition"></i>
              <div class="service-details">
                <h4>What We Do</h4>
                <p>We execute data driven programs, provide peer-to-peer recognition, identify best practices, and enable social recognition to deliver measurable results.</p>
              </div>
            </div>
          </div>
          
          <div class="col-md-4 col-sm-12 col-xs-12">
            <div class="service-wrapper wow fadeIn">
              <i class="flaticon-content"></i>
              <div class="service-details">
                <h4>Our Mission</h4>
                <p>Our mission is to change the way companies work. We believe employees are any company’s greatest assets. By using our technology, companies will empower every employee with the tools and insights to succeed, and create a culture of achievement. </p>
              </div>
            </div>
          </div>
          
          <div class="col-md-4 col-sm-12 col-xs-12 last">
            <div class="service-wrapper wow fadeIn">
              <i class="flaticon-html"></i>
              <div class="service-details">
                <h4>Our Vision</h4>
                <p>Our vision is to continue to provide the world’s most innovative workforce engagement and social recognition software.</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </Layout>
)

export default IndexPage
