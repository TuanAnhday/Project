import axios from 'axios'
import {AppConfigModel} from '../models/ApplicationModel'

const URL_PREFIX = `${process.env.REACT_APP_API_URL}/api/application`

export function getAppConfig() {
  return axios.get<AppConfigModel>(`${URL_PREFIX}/config`)
}
