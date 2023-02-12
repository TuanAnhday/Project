import {configureStore} from '@reduxjs/toolkit'
import createSagaMiddleware from 'redux-saga'
import {rootReducer, rootSaga} from './RootReducer'
import {reduxBatch} from '@manaflair/redux-batch'
import persistStore from 'redux-persist/es/persistStore'

const sagaMiddlerware = createSagaMiddleware()

const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) => [
    ...getDefaultMiddleware({
      immutableCheck: false,
      serializableCheck: false,
      thunk: true,
    }),
    sagaMiddlerware,
  ],
  devTools: process.env.NODE_ENV !== 'production',
  enhancers: [reduxBatch],
})

export type AppDispatch = typeof store.dispatch

export const persistor = persistStore(store)
sagaMiddlerware.run(rootSaga)
export default store
