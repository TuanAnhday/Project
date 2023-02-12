export default function setupAxios(axios: any, store: any) {
  axios.defaults.headers.Accept = 'application/json;charset=utf-8'
  axios.defaults.headers.post['Access-Control-Allow-Origin'] = '*'
  axios.interceptors.request.use(
    (config: any) => {
      const {
        auth: {accessToken},
      } = store.getState()

      if (accessToken) {
        config.headers.Authorization = `Bearer ${accessToken}`
      }
      return config
    },
    (error: any) => Promise.reject(error)
  )
  axios.interceptors.response.use(
    (res: any) => res,
    (err: {response: {data: {message: any}; status: number}}) => {
      if (err.response.data.message) {
        // toast.error(err.response.data.message);
        if (err.response.status === 401) {
          store.dispatch({
            type: 'auth/LOGOUT',
          })
        }
      }
      return Promise.reject(err)
    }
  )
}
