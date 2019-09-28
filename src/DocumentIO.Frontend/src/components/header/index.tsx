import React, { PropsWithChildren } from 'react'
import { Menu, Dropdown, Header } from "semantic-ui-react"
import { Link } from 'react-router-dom'

export const MenuHeader = (props: PropsWithChildren<{}>) => {

	return (
		<Menu size='small'>
			<Menu.Item>
				<Header as={Link} to='/'>
					DocumentIO
				</Header>
			</Menu.Item>

			<Menu.Menu position='right'>
				{ props.children }
			</Menu.Menu>
		</Menu>
	)
}

export const MenuDropdown = () => (
	<Menu.Menu position='right'>
		<Dropdown item text='Аккаунт'>
			<Dropdown.Menu>
				<Dropdown.Item>Дашборд</Dropdown.Item>
				<Dropdown.Item>Мои карточки</Dropdown.Item>
				<Dropdown.Item>Уведомления</Dropdown.Item>
				<Dropdown.Divider/>
				<Dropdown.Item>Настройки</Dropdown.Item>
			</Dropdown.Menu>
		</Dropdown>
	</Menu.Menu>
)