import React from 'react'
import {useSelector} from 'react-redux'
import {shallowEqual} from 'react-redux/es/exports'
import {BrowserRouter, Route, Routes} from 'react-router-dom'
import App from '../App'
import {AuthPage} from '../component/auth'
import {UserModel} from '../component/auth/models/UserModel'
import {RootState} from '../setup'

const {PUBLIC_URL} = process.env

function AppRouters() {
  const user = useSelector<RootState, UserModel>(({auth}) => auth.user as UserModel, shallowEqual)

  return (
    <BrowserRouter>
      <Routes>
        <Route path='*' element={<AuthPage />}></Route>
        <Route element={<App />}></Route>
      </Routes>
    </BrowserRouter>
  )
}

export default AppRouters
