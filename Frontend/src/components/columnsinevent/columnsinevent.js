import React, { useState, useEffect } from "react";
import { DragDropContext, Draggable, Droppable } from "react-beautiful-dnd";
import ModalExampleScrollingContent from "../popup-task/popup-task";


const itemsFromBackend = [
    { id: '1', content: "First task" },
    { id: '2', content: "Second task" },
    { id: '3', content: "Third task" },
    { id: '4', content: "Fourth task" },
    { id: '5', content: "Fifth task" }
];



const onDragEnd = (result, columns, setColumns) => {
    if (!result.destination) return;
    const { source, destination } = result;

    if (source.droppableId !== destination.droppableId) {
        const sourceColumn = columns[source.droppableId];
        const destColumn = columns[destination.droppableId];
        const sourceItems = [...sourceColumn.items];
        const destItems = [...destColumn.items];
        const [removed] = sourceItems.splice(source.index, 1);
        destItems.splice(destination.index, 0, removed);
        setColumns({
            ...columns,
            [source.droppableId]: {
                ...sourceColumn,
                items: sourceItems
            },
            [destination.droppableId]: {
                ...destColumn,
                items: destItems
            }
        });
    } else {
        const column = columns[source.droppableId];
        const copiedItems = [...column.items];
        const [removed] = copiedItems.splice(source.index, 1);
        copiedItems.splice(destination.index, 0, removed);
        setColumns({
            ...columns,
            [source.droppableId]: {
                ...column,
                items: copiedItems
            }
        });
    }
};

function ColumnsCards(props) {
    console.log(props.location.state.id);
    const [columns, setColumns] = useState([]);
    console.log(props);
    
    let columnsFromBackend = {
        '0': {
            name: "Requested",
            items: []
        },
        '1': {
            name: "To do",
            items: []
        },
        '2': {
            name: "In Progress",
            items: []
        },
        '3': {
            name: "Done",
            items: []
        }
    };

  useEffect(() => {
        (async () => {
            let response = await fetch(`http://cv-dentistry.ru/api/Task/items/1/15?filter_IsDeleted=false`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${props.location.state.token}`
                },

            })
            if (response.ok) {
                let res = await response.json()
                console.log(res);
                Object.entries(columnsFromBackend).map(([columnId, column]) => {
                    res.result.items.map((item) => {
                        if (parseInt(columnId) === item.state && props.location.state.id === item.eventId) {
                            column.items.push(item)
                        }
                    })
                })
                setColumns(columnsFromBackend)
            }
        })()
    }, [])

  return (
        <div style={{ display: "flex", justifyContent: "center", height: "100%" }}>
            <DragDropContext
                onDragEnd={result => onDragEnd(result, columns, setColumns)}
            >
                {Object.entries(columns).map(([columnId, column], index) => {
                    return (
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "column",
                                alignItems: "center"
                            }}
                            key={columnId}
                        >
                            <h2>{column.name}</h2>
                           <ModalExampleScrollingContent state={columnId} token={props.location.state.token} id={props.location.state.id} userid={props.location.state.userid}/>
                            <div style={{ margin: 8 }}>
                                <Droppable droppableId={columnId} key={columnId}>
                                    {(provided, snapshot) => {
                                        return (
                                            <div
                                                {...provided.droppableProps}
                                                ref={provided.innerRef}
                                                style={{
                                                    background: snapshot.isDraggingOver
                                                        ? "lightblue"
                                                        : "lightgrey",
                                                    padding: 4,
                                                    width: 250,
                                                    minHeight: 500
                                                }}
                                            >
                                                {column.items.map((item, index) => {
                                                    return (
                                                        <Draggable
                                                            key={item.id}
                                                            draggableId={item.id}
                                                            index={index}
                                                        >
                                                            {(provided, snapshot) => {
                                                                return (
                                                                    <div
                                                                        ref={provided.innerRef}
                                                                        {...provided.draggableProps}
                                                                        {...provided.dragHandleProps}
                                                                        style={{
                                                                            userSelect: "none",
                                                                            padding: 16,
                                                                            margin: "0 0 8px 0",
                                                                            minHeight: "50px",
                                                                            backgroundColor: snapshot.isDragging
                                                                                ? "#263B4A"
                                                                                : "#456C86",
                                                                            color: "white",
                                                                            ...provided.draggableProps.style
                                                                        }}
                                                                    >
                                                                        <p>{item.name}</p>
                                                                        <p>{item.description}</p>
                                                                    </div>
                                                                );
                                                            }}
                                                        </Draggable>
                                                    );
                                                })}
                                                {provided.placeholder}
                                            </div>
                                        );
                                    }}
                                </Droppable>
                            </div>
                        </div>
                    );
                })}
            </DragDropContext>
        </div>
    );
}

export default ColumnsCards;
