import React from "react"
import { getProfile } from "../services/auth"

const Profile = () => (
  <>
    <h1>Your profile</h1>
    <ul>
      <li>Name: {getProfile().name}</li>
      <li>E-mail: {getProfile().email}</li>
    </ul>
  </>
)

export default Profile