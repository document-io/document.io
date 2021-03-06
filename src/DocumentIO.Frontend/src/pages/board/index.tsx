import React, { useState } from 'react'
import { RouteChildrenProps } from 'react-router'
// @ts-ignore
import TrelloBoard from 'react-trello'
import { DocumentIOMenu } from '../../components/menu'
import { useMutation, useQuery } from '@apollo/react-hooks'
import {
  CreateCard,
  CreateCardVariables,
  CreateColumn,
  CreateColumnVariables,
  DeleteCard,
  DeleteCardVariables,
  DeleteColumn,
  DeleteColumnVariables
} from './mutations'
import { CreateCardType, CreateColumnType } from './types'
import { Columns, ReadBoardsVariables } from './queries'
import { CardModal } from '../../components/modal'

export const BoardPage = (props: RouteChildrenProps) => {
  // @ts-ignore
  const boardId = props.match.params.boardId

  const { data, loading, refetch, error } = useQuery<ReadBoardsVariables>(Columns, {
    variables: {
      boardId
    }
  })
  const initialData = {
    lanes: []
  }
  let updatedData = initialData
  if (!loading) {
    updatedData = {
      // @ts-ignore
      lanes: data!.boards[0].columns.map(({ cards, id, name }) => ({
        id,
        title: name,
        cards: cards.map(({ name, description, id }) => ({
          id,
          title: name,
          description: description
        }))
      }))
    }
  }

  const [createColumn] = useMutation<CreateColumnType, CreateColumnVariables>(CreateColumn)
  const [createCard] = useMutation<CreateCardType, CreateCardVariables>(CreateCard)
  const [deleteColumn] = useMutation<{ id: string }, DeleteColumnVariables>(DeleteColumn)
  const [deleteCard] = useMutation<{ id: string }, DeleteCardVariables>(DeleteCard)

  const [cardId, setCardId] = useState('')


  const onLaneAdd = async ({ title }: { title: string }) => {
    try {
      await createColumn({
        variables: {
          column: {
            boardId,
            name: title
          }
        }
      })
      await refetch()
    } catch (e) {

    }
  }

  const onLaneDelete = async (laneId: string) => {
    try {
      await deleteColumn({
        variables: {
          column: {
            id: laneId
          }
        }
      })
      await refetch()
    } catch (e) {

    }
  }

  const onCardAdd = async ({ title }: { title: string }, laneId: any) => {
    try {
      await createCard({
        variables: {
          card: {
            columnId: laneId,
            name: title
          }
        }
      })
      await refetch()
    } catch (e) {
    }
  }

  const onCardDelete = async (cardId: string) => {
    try {
      await deleteCard({
        variables: {
          card: {
            id: cardId
          }
        }
      })
      await refetch()
    } catch (e) {

    }
  }

  const onCardClick = (cardId: string, metadata: any, laneId: string) => {
    onCard(cardId)
  }

  const onCard = (cardId: string) => {
    setCardId(cardId)
  }

  return loading ? null : (
    <>
      {cardId === '' ? null : <CardModal cardId={cardId} onCard={onCard}/>}
      <DocumentIOMenu logoUrl='/dashboard' search dropdown {...props}/>
      <TrelloBoard
        data={updatedData}
        editable
        canAddLanes
        laneDraggable={false}
        cardDraggable={false}
        onLaneAdd={onLaneAdd}
        onCardAdd={onCardAdd}
        onCardDelete={onCardDelete}
        onLaneDelete={onLaneDelete}
        onCardClick={onCardClick}
      />
    </>
  )
}