import {combineReducers} from 'redux'
import {all} from 'redux-saga/effects'

import * as auth from '../../component/auth'

export const rootReducer = combineReducers({
  auth: auth.reducer,
})

export type RootState = ReturnType<typeof rootReducer>

export function* rootSaga() {
  yield all([auth.saga()])
}
