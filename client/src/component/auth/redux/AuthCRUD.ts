import axios from "axios"

const URL = `https://localhost:44395/api/Users`
const URL_CONFIG = `${process.env.REACT_APP_API_URL}/api/Users`

export const login = (userName : string, password: string) => {
    console.log(URL);
    
    return axios.post(`${URL}/login`,{username : userName, password})
}