export interface UserModel {
  userName: string
  password?: string
  fullName: string
  dob: string
  createDate: string | Date
  isActive: boolean
  phoneNumber: string
}
