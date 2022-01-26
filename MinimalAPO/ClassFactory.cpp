#include "stdafx.h"
#include "MinimalAPO.h"
#include "ClassFactory.h"

long ClassFactory::lockCount = 0;

ClassFactory::ClassFactory()
{
	refCount = 1;
}

HRESULT __stdcall ClassFactory::QueryInterface(const IID& iid, void** ppv)
{
	if (iid == __uuidof(IUnknown) || iid == __uuidof(IClassFactory))
		*ppv = static_cast<IClassFactory*>(this);
	else
	{
		*ppv = NULL;
		return E_NOINTERFACE;
	}

	reinterpret_cast<IUnknown*>(*ppv)->AddRef();
	return S_OK;
}

ULONG __stdcall ClassFactory::AddRef()
{
	return InterlockedIncrement(&refCount);
}

ULONG __stdcall ClassFactory::Release()
{
	if (InterlockedDecrement(&refCount) == 0)
	{
		delete this;
		return 0;
	}

	return refCount;
}

HRESULT __stdcall ClassFactory::CreateInstance(IUnknown* pUnknownOuter, const IID& iid, void** ppv)
{
	if (pUnknownOuter != NULL && iid != __uuidof(IUnknown))
		return E_NOINTERFACE;

	MinimalAPO* apo = new MinimalAPO(pUnknownOuter);
	if (apo == NULL)
		return E_OUTOFMEMORY;

	HRESULT hr = apo->NonDelegatingQueryInterface(iid, ppv);

	apo->NonDelegatingRelease();
	return hr;
}

HRESULT __stdcall ClassFactory::LockServer(BOOL bLock)
{
	if (bLock)
		InterlockedIncrement(&lockCount);
	else
		InterlockedDecrement(&lockCount);

	return S_OK;
}
