using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Command
{
    private bool m_IsRunning = false;

    protected TankController m_Controller;
    protected bool m_IsBlocking = false;

    public bool IsRunning
    {
        get
        {
            return m_IsRunning;
        }
    }
    public bool IsBlocking
    {
        get
        {
            return m_IsBlocking;
        }
    }

    protected Command(TankController controller)
    {
        m_Controller = controller;
    }

    public virtual void Execute()
    {
        m_IsRunning = true;
    }
    public virtual bool IsFinished()
    {
        return true;
    }
}

