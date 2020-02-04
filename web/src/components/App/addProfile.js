import React from 'react';
import { tokens, setUser } from '../../services/auth';
import { navigate } from '@reach/router';

export default class AddProfile extends React.Component {
  handleInputChange = event => {
    const target = event.target;
    const value = target.value;
    const name = target.name;

    this.setState({
      hasUpdates: true,
      profile: { ...this.state.profile, [name]: value },
    });
  };

  handleSubmit = event => {
    event.preventDefault();

    const apiUrl = process.env.GATSBY_ADD_TEMP_USER_FUNCTION_ENDPOINT;

    const options = {
      method: 'PUT',
      body: JSON.stringify(this.state.profile),
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${tokens.accessToken}`,
      },
    };

    fetch(apiUrl, options)
      .then(
        response => {
          if (response.ok) {
            return response.json();
          } else {
            return null;
          }
        },
        error => {
          console.error(error);
          navigate('/app/profileupdateerror/');
        }
      )
      .then(data => {
        if (data !== null) {
          setUser(data.result);
          navigate('/app/app_profile_created/');
        } else {
          navigate('/app/profileupdateerror/');
        }
      });
  };

  constructor() {
    super();

    this.state = {
      hasUpdates: false,
      profile: {},
    };
  }

  async componentDidMount() {
    
  }

  render() {
    return (
      <section className="section">
        <div className="container">
          <div className="row">
            <div className="col-md-8">
              <h1>Add New profile</h1>
              <form className="contactform" onSubmit={this.handleSubmit}>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="firstName"
                    name="firstName"
                    placeholder="First Name"
                    required
                    value={this.state.profile.given_name || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="lastName"
                    name="lastName"
                    placeholder="Last Name"
                    required
                    value={this.state.profile.family_name || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="fullName"
                    name="fullName"
                    placeholder="Full Name"
                    required
                    value={this.state.profile.name || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="email"
                    name="email"
                    placeholder="Email"
                    required
                    value={this.state.profile.email || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="streetAddress"
                    name="streetAddress"
                    placeholder="Street Address"
                    required
                    value={this.state.profile.streetAddress || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="city"
                    name="city"
                    placeholder="City"
                    required
                    value={this.state.profile.city || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="state"
                    name="state"
                    placeholder="State"
                    required
                    value={this.state.profile.state || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <input
                    type="text"
                    className="form-control"
                    id="postalCode"
                    name="postalCode"
                    placeholder="Zip Code"
                    required
                    value={this.state.profile.postalCode || ''}
                    onChange={this.handleInputChange}
                  />
                </div>
                <button type="submit" id="submit" name="submit" className="btn btn-primary">
                  Submit Form
                </button>
              </form>
            </div>
          </div>
        </div>
      </section>
    );
  }
}
