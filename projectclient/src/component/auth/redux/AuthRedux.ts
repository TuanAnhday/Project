import {Action} from '@reduxjs/toolkit'
import {UserModel} from '../models/UserModel'
import {persistReducer} from 'redux-persist'
import storage from 'redux-persist/lib/storage'
import {put, takeLatest} from 'redux-saga/effects'
import {getAppConfig} from './ApplicationCRUD'

export interface ActionWithPayload<T> extends Action {
  payload?: T
}

export const actionTypes = {
  Login: '[Login] Action',
  Logout: '[Logout] Action',
  Register: '[Register] Action',
  UserRequested: '[Request User] Action',
  UserLoaded: '[Load User] Auth API',
  SetUser: '[Set User] Action',
}
export interface IAuthState {
  user?: UserModel
  accessToken?: string
}
const initialAuthState: IAuthState = {
  user: undefined,
  accessToken: undefined,
}

export const reducer = persistReducer(
  {storage, key: 'v1-project-auth', whitelist: ['user', 'accessToken']},
  (state: IAuthState = initialAuthState, action: ActionWithPayload<IAuthState>) => {
    switch (action.type) {
      case actionTypes.Login: {
        const accessToken = action.payload?.accessToken
        return {accessToken}
      }
      case actionTypes.Register: {
        const accessToken = action.payload?.accessToken
        return {accessToken}
      }
      case actionTypes.Logout: {
        return initialAuthState
      }
      case actionTypes.UserLoaded: {
        const user = action.payload?.user
        return {...state, user}
      }
      case actionTypes.SetUser: {
        const user = action.payload?.user
        return {...state, user}
      }

      default:
        return state
    }
  }
)
export const actions = {
  login: (accessToken: string) => ({type: actionTypes.Login, payload: {accessToken}}),
  register: (accessToken: string) => ({type: actionTypes.Register, payload: {accessToken}}),
  logout: () => ({type: actionTypes.Logout}),
  requestUser: () => ({type: actionTypes.UserRequested}),
  fulfilUser: (user: UserModel) => ({type: actionTypes.UserLoaded, payload: {user}}),
  setUser: (user: UserModel) => ({type: actionTypes.SetUser, payload: {user}}),
}

export function* saga() {
  yield takeLatest(actionTypes.Login, function* loginSaga() {
    yield put(actions.requestUser())
  })

  yield takeLatest(actionTypes.Register, function* registerSaga() {
    yield put(actions.requestUser())
  })

  yield takeLatest(actionTypes.UserLoaded, function* userRequested() {
    const {
      data: {user},
    } = yield getAppConfig()
    yield put(actions.fulfilUser(user))
  })
}
