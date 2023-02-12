import React from 'react'
import {Route, Routes} from 'react-router-dom'
import Login from './component/Login'

function AuthPage() {
  return (
    <Routes>
      <Route index element={<Login />} />
    </Routes>
  )
}

export {AuthPage}
