import React from "react"

import Layout from "../components/layout"
import SEO from "../components/seo"

const NotFoundPage = () => (
  <Layout>
    <SEO title="404: Not found" />
    <section class="section">
		  <div class="container">
			  <div class="row text-center">
				  <div class="col-md-12 notfound">
					  <h2>404</h2>
						<p class="lead">Sorry we could not reach the page you were looking for. <br />Content has been deleted or modified.</p>
					</div>
				</div>
			</div>
		</section>
  </Layout>
)

export default NotFoundPage