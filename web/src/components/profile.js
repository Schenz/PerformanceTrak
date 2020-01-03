import React from "react"
import { getProfile } from "../services/auth"

const Profile = () => (
  <>
    <h1>Your profile</h1>
    <ul>
      <li>Name: {getProfile().name}</li>
      <li>E-mail: {getProfile().emails[0]}</li>
    </ul>
  </>
)

export default Profile