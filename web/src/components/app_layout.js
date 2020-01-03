import React from "react"
import PropTypes from "prop-types"
import { useStaticQuery, graphql } from "gatsby"
import AppHeader from "./app_header"
import Footer from "./footer"
import "./font-awesome.min.css"
import "./bootstrap.min.css"
import "./animate.css"
import "./carousel.css"
import "./style.css"

const AppLayout = ({ children }) => {
    const data = useStaticQuery(graphql`
        query AppSiteTitleQuery {
            site {
                siteMetadata {
                    title
                }
            }
        }
    `)

    return (
        <>
            <div id="wrapper">
                <AppHeader siteTitle={data.site.siteMetadata.title} />
                {children}
                <Footer />
            </div>
        </>
    )
}

AppLayout.propTypes = {
    children: PropTypes.node.isRequired,
}

export default AppLayout
