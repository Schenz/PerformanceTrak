import React from "react"

import Layout from "../components/layout"
import SEO from "../components/seo"

const ContributionBonus = () => (
    <Layout>
        <SEO title="Contribution Bonus" />
        <section className="section overfree">
            <div className="container">
                <div className="section-title">
                    <h3 className="text-center">Contribution Bonus</h3>
                    <hr />
                    <p className="lead">
                        An extension to the peer-to-peer program, we know
                        recognition from our peers to be an important part of
                        developing a culture of collaboration, support and
                        professional fulfillment. Every year, each salaried
                        consultant and above will receive funds to distribute to
                        other employees to recognize them for the extraordinary
                        impact they have on our organization, our clients or our
                        community. In order to receive the bonus, an employee
                        must be employed at the time the distribution is made
                        and have distributed all their funds.
                    </p>
                    <p></p>
                    <p className="lead">
                        There is only one rule on how you may distribute the
                        bonus: you must give it away. Some distribution options
                        are:
                    </p>
                    <ul className="lead">
                        <li>Award all of the money to 1 person</li>
                        <li>Award the money equally to other employees</li>
                        <li>
                            Award the bulk of the bonus to one and a smaller
                            portion to another
                        </li>
                        <li>
                            All we ask is for you to consider the
                            individual’s contribution to the organization
                            and thank them for his or her contributions to
                            making a significant positive impact!
                        </li>
                    </ul>
                </div>
            </div>
        </section>
    </Layout>
)

export default ContributionBonus
