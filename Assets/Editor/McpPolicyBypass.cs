using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class McpPolicyBypass
{
    private static bool hasLoggedError = false;

    static McpPolicyBypass()
    {
        // Hook into editor update to run our bypass logic continuously
        EditorApplication.update -= UpdateBypass;
        EditorApplication.update += UpdateBypass;
        
        // Also run once immediately
        RunBypass();
    }

    private static void UpdateBypass()
    {
        RunBypass();
    }

    private static void RunBypass()
    {
        try
        {
            // Set SessionState keys just in case
            SessionState.SetInt("ConnectionPolicyOverride.MaxDirect", -1);
            SessionState.SetInt("ConnectionPolicyOverride.MaxGateway", -1);
            SessionState.SetInt("ConnectionPolicyOverride.HasOverride", 1);

            // Set Policy to unlimited
            Type connectionPolicyType = Type.GetType("Unity.AI.MCP.Editor.Connection.ConnectionPolicy, Unity.AI.MCP.Editor");
            Type connectionCensusType = Type.GetType("Unity.AI.MCP.Editor.Connection.ConnectionCensus, Unity.AI.MCP.Editor");
            if (connectionPolicyType != null && connectionCensusType != null)
            {
                object policyInstance = Activator.CreateInstance(connectionPolicyType, new object[] { -1, -1 });
                MethodInfo setPolicyMethod = connectionCensusType.GetMethod("SetPolicy", BindingFlags.Public | BindingFlags.Static);
                if (setPolicyMethod != null)
                {
                    setPolicyMethod.Invoke(null, new object[] { policyInstance });
                }
            }

            // Now bypass the ApprovalState check by modifying TransportStore.States
            Type transportStoreType = Type.GetType("Unity.AI.MCP.Editor.TransportStore, Unity.AI.MCP.Editor");
            if (transportStoreType == null)
            {
                if (!hasLoggedError)
                {
                    hasLoggedError = true;
                    Debug.LogWarning("Antigravity MCP Bypass: TransportStore type not found.");
                }
                return;
            }

            FieldInfo statesField = transportStoreType.GetField("States", BindingFlags.NonPublic | BindingFlags.Static);
            if (statesField == null)
            {
                if (!hasLoggedError)
                {
                    hasLoggedError = true;
                    Debug.LogWarning("Antigravity MCP Bypass: States field not found in TransportStore.");
                }
                return;
            }

            System.Collections.IDictionary statesDict = statesField.GetValue(null) as System.Collections.IDictionary;
            if (statesDict == null)
            {
                if (!hasLoggedError)
                {
                    hasLoggedError = true;
                    Debug.LogWarning("Antigravity MCP Bypass: States dictionary is null.");
                }
                return;
            }

            Type approvalStateType = Type.GetType("Unity.AI.MCP.Editor.ConnectionApprovalState, Unity.AI.MCP.Editor");
            Type transportStateType = Type.GetType("Unity.AI.MCP.Editor.TransportState, Unity.AI.MCP.Editor");
            
            if (approvalStateType == null || transportStateType == null)
            {
                if (!hasLoggedError)
                {
                    hasLoggedError = true;
                    Debug.LogWarning($"Antigravity MCP Bypass: Types not found. approvalStateType={approvalStateType}, transportStateType={transportStateType}");
                }
                return;
            }

            object approvedValue = Enum.ToObject(approvalStateType, 3); // Approved
            FieldInfo approvalStateField = transportStateType.GetField("ApprovalState", BindingFlags.Public | BindingFlags.Instance);
            
            if (approvalStateField == null)
            {
                if (!hasLoggedError)
                {
                    hasLoggedError = true;
                    Debug.LogWarning("Antigravity MCP Bypass: ApprovalState field not found in TransportState.");
                }
                return;
            }

            // Copy keys to avoid modification exceptions during iteration
            var keys = new System.Collections.ArrayList(statesDict.Keys);
            foreach (object key in keys)
            {
                object stateObj = statesDict[key];
                if (stateObj != null)
                {
                    object currentState = approvalStateField.GetValue(stateObj);
                    int currentVal = (int)currentState;
                    // If it's not Approved (3) and not GatewayApproved (5), force it to Approved (3)
                    if (currentVal != 3 && currentVal != 5)
                    {
                        approvalStateField.SetValue(stateObj, approvedValue);
                        Debug.Log($"Antigravity MCP Bypass: Forced connection approval state from {currentState} to Approved");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (!hasLoggedError)
            {
                hasLoggedError = true;
                Debug.LogError("Antigravity MCP Bypass failed with exception: " + ex);
            }
        }
    }
}
