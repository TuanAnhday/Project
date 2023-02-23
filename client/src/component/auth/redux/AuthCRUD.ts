import axios from "axios"

const URL = `https://localhost:44395`
const URL_CONFIG = `${URL}/api/Users`

export const login = (userName : string, password: string) => {
    console.log(URL);
    
    return axios.post(`${URL_CONFIG}/login`,{username : userName, password})
}