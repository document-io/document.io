import 'semantic-ui-css/semantic.min.css'
import React from 'react'
import ReactDOM from 'react-dom'

import ApolloClient from 'apollo-boost'
import { ApolloProvider } from '@apollo/react-hooks'

import { Router } from './router'
import { BrowserRouter } from 'react-router-dom'

import './index.scss'

const graphql = new ApolloClient({
  uri: '/graphql'
})

ReactDOM.render(
  <ApolloProvider client={graphql}>
    <BrowserRouter>
      <Router/>
    </BrowserRouter>
  </ApolloProvider>,
  document.getElementById('root')
)