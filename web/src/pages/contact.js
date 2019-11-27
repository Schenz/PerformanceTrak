import React from "react"

import Layout from "../components/layout"
import SEO from "../components/seo"

const Contact = props => (
  <Layout>
    <SEO title="Contact" />
    <section class="section transheader bgcolor">
			<div class="container">
				<div class="row">	
					<div class="col-md-10 col-md-offset-1 col-sm-12 text-center">
						<h2>Contact Us</h2>
						<p class="lead">We offer the best data driven programs, peer-to-peer recognition, best practices, and social recognition.</p>
					</div>
				</div>
			</div>
		</section>

		<section class="section">
			<div class="container">
				<div class="row">
					<div class="col-md-4">
						<div class="contact-details">
							<p>If you have questions about our security or privacy terms of service, let us know immediately.</p>
							<p>Please use the options below to contact us.</p>
							<hr />
						</div>
					</div>

					<div class="col-md-5">
						<form class="contactform">
						    <div class="form-group">
						        <input type="text" class="form-control" id="name" name="name" placeholder="Name" required />
						    </div>
						    <div class="form-group">
						        <input type="text" class="form-control" id="email" name="email" placeholder="Email" required />
						    </div>
						    <div class="form-group">
						        <input type="text" class="form-control" id="phone" name="phone" placeholder="Phone" required />
						    </div>
						    <div class="form-group">
						        <input type="text" class="form-control" id="subject" name="subject" placeholder="Subject" required />
						    </div>
						    <div class="form-group">
						        <textarea class="form-control" id="message" placeholder="Message" maxlength="140" rows="7"></textarea>
						    </div>
						    <button type="button" id="submit" name="submit" class="btn btn-primary">Submit Form</button>
						</form>
					</div>

				</div>
			</div>
		</section>
  </Layout>
)

export default Contact;