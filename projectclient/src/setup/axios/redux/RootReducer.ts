import { combineReducers } from 'redux'
import {all} from 'redux-saga/effects'

import * as auth from './../../../compenent/auth'

export const rootReducer = combineReducers({
    auth : auth.reducer
})

export type RootState = ReturnType<typeof rootReducer>

export function* rootSage(){
    yield all([auth.saga()])
}