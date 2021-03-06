import React, {useEffect, useState} from "react"
import {useQuery} from "@apollo/react-hooks"
import {Accounts, ReadAccountsResponse} from "./queries"
import {Dimmer, Loader, Message, Card, Icon} from "semantic-ui-react"
import {RouteChildrenProps} from "react-router"

export const DashboardUsersTab = (props: RouteChildrenProps) => {
    const [globalValidation, setGlobalValidation] = useState('')

    const {loading, error, data} = useQuery<ReadAccountsResponse>(Accounts)

    useEffect(() => {
        setGlobalValidation(error ? error.graphQLErrors[0].message : '')
    }, [error])

    if (loading) {
        return (
            <Dimmer active inverted>
                <Loader size='big'/>
            </Dimmer>
        )
    }

    // @ts-ignore
    const items = data.accounts
        .map(account => (
            <Card key={account.id} centered onClick={() => props.history.push(`/profile/${account.id}`)}>
                <Card.Content>
                    <Card.Header>
                        Логин: {account.login}
                    </Card.Header>
                    <Card.Meta>
                        Роль: {account.role}
                    </Card.Meta>
                    <Card.Description>
                        ФИО: {account.firstName} {account.lastName}
                    </Card.Description>
                </Card.Content>
                <Card.Content extra>
                    <Icon name='at'/>{account.email}
                </Card.Content>
            </Card>
        ))

    return (
        <React.Fragment>
            {
                globalValidation === ''
                    ? null
                    : (
                        <Message
                            error
                            content={globalValidation}/>
                    )
            }

            <Card.Group>
                {items}
            </Card.Group>
        </React.Fragment>
    )
}