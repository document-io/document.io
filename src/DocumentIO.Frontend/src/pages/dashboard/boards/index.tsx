import React, { useEffect, useState } from "react"
import { useQuery } from "@apollo/react-hooks"
import { Boards, ReadBoardsResponse } from "./queries"
import { Dimmer, Loader, Message, Card, Icon, Header, Input, Form, Grid } from "semantic-ui-react"

export const DashboardBoardsTab = () => {
	const [globalValidation, setGlobalValidation] = useState("")

	const { loading, error, data } = useQuery<ReadBoardsResponse>(Boards)

	useEffect(() => {
		setGlobalValidation(error ? error.graphQLErrors[0].message : "")
	}, [error])

	if (loading) {
		return (
			<Dimmer active inverted>
				<Loader size='big' />
			</Dimmer>
		)
	}

	 // @ts-ignore
	const items = data.boards
		.map(board => (
			<Card key={board.id} centered>
				<Card.Content>
					<Card.Header>{board.name}</Card.Header>
					<Card.Description>Leverage agile frameworks to provide a robust synopsis for high level overviews.</Card.Description>
				</Card.Content>
				<Card.Content extra>
					<Icon name='user' />4 Friends
				</Card.Content>
			</Card>
		))
		.concat((
			<Card key='addBoard' centered>
				
				<Card.Content>
					<Card.Header>Добавить</Card.Header>
					<Card.Description>
						<Form>
							<Form.Input fluid placeholder='Введите название'/>
						</Form>
					</Card.Description>
				</Card.Content>
				<Card.Content extra>
					Нажмите enter
				</Card.Content>
			</Card>
		))
	
	return (
		<React.Fragment>
			{
				globalValidation === ""
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