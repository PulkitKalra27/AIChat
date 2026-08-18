import {useState} from 'react'
import MessageList from './MessageList'
import ChatInput from './ChatInput'

function Chat()
{
    const [message ,setMessage] = useState('')
    const [messages, setMessages] = useState([])

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
        setMessages([...messages,userMessage,assistantMessage])
        setMessage('')
        const response = await fetch('https://localhost:7245/api/chat/stream',{
            method:'POST',
            headers:{
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                message:message
            })
        })
        const reader = response.body.getReader()
        const decoder = new TextDecoder()
        let buffer = ''

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

                let text = event.slice(5)

                if (text.startsWith(' '))
                {
                    text = text.slice(1)
                }

                setMessages(prevMessages =>
                    prevMessages.map((msg, index) =>
                        index === prevMessages.length - 1
                            ? {
                                ...msg,
                                content: msg.content + text
                            }
                            : msg
                    )
                )
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
        <div className="chat">
            <div className="chat-header">
                <h1>AI Chat</h1>
            </div>
            <MessageList messages={messages}/>
            <ChatInput message={message} setMessage={setMessage} handleSend={handleSend}/>
        </div>
    )
}
export default Chat