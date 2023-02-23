import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import reportWebVitals from './reportWebVitals'
import store from './setup/redux/Store'
import {Provider} from 'react-redux'
import AppRouters from './routing/AppRouters'
import 'bootstrap/dist/css/bootstrap.min.css'
import './style.scss'

import * as _redux from './setup'
import axios from 'axios'

_redux.setupAxios(axios, store)
const root = ReactDOM.createRoot(document.getElementById('root') as HTMLElement)
root.render(
  <React.StrictMode>
    <Provider store={store}>
      <AppRouters />
    </Provider>
  </React.StrictMode>
)

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals()
