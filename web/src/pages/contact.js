import React from "react"

import Layout from "../components/layout"
import SEO from "../components/seo"

export default class Contact extends React.Component {
	state = {
		name: "",
		email: "",
		phone: "",
		subject: "",
		message: ""
	}
	
	handleInputChange = event => {
		const target = event.target
		const value = target.value
		const name = target.name
		this.setState({
			[name]: value,
		})
	}
	
	handleSubmit = event => {
		event.preventDefault();
		console.log(process);
		console.log(process.env);
		console.log(process.env.GATSBY_EMAIL_FUNCTION_ENDPOINT);
		alert(`Welcome ${this.state.name} ${this.state.email}!`)
	}

	render() {
		return (
			<Layout>
    			<SEO title="Contact" />
    			<section className="section transheader bgcolor">
					<div className="container">
						<div className="row">	
							<div className="col-md-10 col-md-offset-1 col-sm-12 text-center">
								<h2>Contact Us</h2>
								<p className="lead">We offer the best data driven programs, peer-to-peer recognition, best practices, and social recognition.</p>
							</div>
						</div>
					</div>
				</section>

				<section className="section">
					<div className="container">
						<div className="row">
							<div className="col-md-4">
								<div className="contact-details">
									<p>If you have questions about our security or privacy terms of service, let us know immediately.</p>
									<p>Please use the options below to contact us.</p>
									<hr />
								</div>
							</div>

							<div className="col-md-5">
								<form className="contactform" onSubmit={this.handleSubmit}>
				    				<div className="form-group">
				        				<input type="text" className="form-control" id="name" name="name" placeholder="Name" required value={this.state.name} onChange={this.handleInputChange} />
				    				</div>
				    				<div className="form-group">
				        				<input type="text" className="form-control" id="email" name="email" placeholder="Email" required value={this.state.email} onChange={this.handleInputChange} />
				    				</div>
				    				<div className="form-group">
				        				<input type="text" className="form-control" id="phone" name="phone" placeholder="Phone" required value={this.state.phone} onChange={this.handleInputChange} />
				    				</div>
				    				<div className="form-group">
				        				<input type="text" className="form-control" id="subject" name="subject" placeholder="Subject" required value={this.state.subject} onChange={this.handleInputChange} />
				    				</div>
				    				<div className="form-group">
				        				<textarea className="form-control" id="message" placeholder="Message" maxLength="140" rows="7" value={this.state.message} onChange={this.handleInputChange}></textarea>
				    				</div>
				    				<button type="submit" id="submit" name="submit" className="btn btn-primary">Submit Form</button>
								</form>
							</div>
						</div>
					</div>
				</section>
  			</Layout>
	  	)
	}
}