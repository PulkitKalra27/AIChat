import {useEffect,useState} from 'react'
import MessageList from './MessageList'
import ChatInput from './ChatInput'

function Chat()
{
    const [message ,setMessage] = useState('')
    const [messages, setMessages] = useState([])

    const [conversations,setConversations] = useState([])
    const [selectConversationId, setSelectConversationId] = useState(null)
    function handleNewChat(){
        setSelectConversationId(null)
        setMessages([])
    }
    async function loadConversations(){
        const response = await fetch('https://localhost:7245/api/Conversation')
        const data = await response.json()
        setConversations(data)
    }
    useEffect(()=>{loadConversations()},[])
    async function loadMessages(id){
        setSelectConversationId(id)
        const response = await fetch(`https://localhost:7245/api/Conversation/${id}/messages`)
        const data = await response.json()
        setMessages(data)
    }
    async function handleSend(){
        if(!message.trim()){
            return
        }
        const userMessage ={
                role:'user', 
                content:message
            }
        const assistantMessage =
        {
                role:'assistant', 
                content:''
            
        }
        setMessages(prevMessages => [...prevMessages, userMessage, assistantMessage])
        setMessage('')
        const response = await fetch('https://localhost:7245/api/chat/stream',{
            method:'POST',
            headers:{
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                conversationid: selectConversationId,
                message:message
            })
        })
        const reader = response.body.getReader()
        const decoder = new TextDecoder()
        let buffer = ''
        let isNewChat = !selectConversationId;
        while (true)
        {
            const { value, done } = await reader.read()

            if (done)
            {
                break
            }

            buffer += decoder.decode(value, { stream: true })

            const events = buffer.split('\n\n')

            buffer = events.pop()

            for (const event of events)
            {
                if (!event.startsWith('data:'))
                {
                    continue
                }

                let data = event.slice(5).trim()
                if(data === '[DONE]')
                {
                    continue
                }
                const ChatStreamEvent = JSON.parse(data) 
                if(ChatStreamEvent.type === 'conversation')
                    {
                        // console.log('Conversation ID:', ChatStreamEvent.conversationId)
                        setSelectConversationId(ChatStreamEvent.conversationId)
                        if (isNewChat){
                            loadConversations()
                            isNewChat = false
                        }
                    }
                if(ChatStreamEvent.type === 'content')
                    {
                        setMessages(prevMessages => prevMessages.map((msg, index) =>
                                index === prevMessages.length - 1? 
                                {...msg,content: msg.content + ChatStreamEvent.content}: msg
                            )
                        )

                    }   
            }
        }
        // while(true)
        // {
        //     const {value, done} = await reader.read()
        //     if(done)
        //     {
        //         break
        //     }
        //     const chunk = decoder.decode(value)
        //     const text = chunk.replace('data:' , '').trim()
        //     setMessages(prevMessages => prevMessages.map((msg,index) => 
        //         index=== prevMessages.length - 1 ? 
        //         {...msg, content: msg.content + text} : msg))
        //     // console.log(chunk)
        // }
        // const data = await response.json()
        // setMessages([...messages,userMessage,
        //     {
        //         role:'assistant',
        //         content:data.response
        //     }
        // ])
    }
    return(
        <div className="chat-layout">
            <div className="sidebar">
                <div className="sidebar-header">
                    <h1>AI Chat</h1>
                </div>
                <div className="conversation-list">
                    <button onClick={handleNewChat}>+ New Chat</button>
                </div>
                <div className="conversation-separator">
                        <h1>Recent Chats</h1>
                </div>
                {conversations.map(conversation =>(
                    <div
                    key = {conversation.id}
                    className={`conversation-item ${selectConversationId === conversation.id ? 'selected' : ''}`}
                    onClick={() => loadMessages(conversation.id)} >
                        {conversation.title}
                    </div>
                ))}
                {/*
                    <div className="conversation-item">
                        What is RAG?
                    </div>

                    <div className="conversation-item">
                        What are embeddings?
                    </div>

                    <div className="conversation-item">
                        Explain APIs
                    </div>
                </div> */}
            </div>    
            <div className="chat">
                
                <MessageList messages={messages}/>
                <ChatInput message={message} setMessage={setMessage} handleSend={handleSend}/>
            </div>
        </div>
    )
}
export default Chat