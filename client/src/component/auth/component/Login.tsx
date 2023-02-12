import {Formik, Field, Form} from 'formik'
import React from 'react'
import {Row, Col, Button} from 'react-bootstrap'
import {useDispatch} from 'react-redux'
import * as Yup from 'yup'
import {login} from '../redux/AuthCRUD'
import './style/style.scss'
import * as auth from '../redux/AuthRedux'
import {toast} from 'react-toastify'
function Login() {
  const dispatch = useDispatch()

  const loginSchema = Yup.object().shape({
    userNameA: Yup.string().required('Nhập tên đăng nhập'),
    password: Yup.string().required('Nhập mật khẩu'),
  })
  const handleSubmit = (values: any) => {
    login(values.userNameA, values.password)
      .then((res) => dispatch(auth.actions.login(values.accessToken)))
      .catch(() => {
        toast.error('Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng đăng nhập lại !!!')
      })
  }
  return (
    <>
      <Formik
        enableReinitialize
        initialValues={{
          userNameA: '',
          password: '',
        }}
        validationSchema={loginSchema}
        onSubmit={handleSubmit}
      >
        {({values, errors, setFieldValue, touched, setTouched}) => (
          <Form>
            <Row className='container login-container align-items-center justify-content-center'>
              <div>
                <Row>
                  <h1>TEST KEY MANAGEMENT</h1>
                  <div>
                    <h3>Đăng nhập</h3>
                    <span>Thông tin đăng nhập hệ thống</span>
                    <hr />
                  </div>
                </Row>
                <Row>
                  <div className='form-label required'>Tên đăng nhập</div>
                  <Field
                    className='form-control'
                    name='userNameA'
                    autoComplete={'new-password'}
                    onChange={(e: any) => setFieldValue('userNameA', e.target.value)}
                    onBlur={() => setTouched({...touched, userNameA: true})}
                    onFocus={() => setTouched({...touched, userNameA: false})}
                  />
                  {errors.userNameA && touched.userNameA && (
                    <div className='error-text'>{errors.userNameA}</div>
                  )}
                </Row>
                <Row>
                  <div className='form-label required'>Mật khẩu</div>
                  <Field
                    type='password'
                    className='form-control'
                    name='password'
                    autoComplete='new-password'
                    onChange={(e: any) => setFieldValue('password', e.target.value)}
                    onBlur={() => setTouched({...touched, password: true})}
                    onFocus={() => setTouched({...touched, password: false})}
                  ></Field>
                  {errors.password && touched.password && (
                    <div className='error-text'>{errors.password}</div>
                  )}
                </Row>
                <Row className='justify-content-center align-item-center mt-3'>
                  <Button variant='success' className='w-50' type='submit'>
                    Đăng nhập
                  </Button>
                </Row>
              </div>
            </Row>
          </Form>
        )}
      </Formik>
    </>
  )
}

export default Login
